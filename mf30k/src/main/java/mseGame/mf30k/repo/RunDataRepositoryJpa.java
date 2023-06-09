package mseGame.mf30k.repo;

import java.util.List;
import java.util.Optional;
import java.util.function.Function;
import java.util.Date;

import org.springframework.data.domain.Example;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.query.FluentQuery.FetchableFluentQuery;
import org.springframework.stereotype.Repository;

public interface RunDataRepositoryJpa extends JpaRepository<RunData, Long>{

}
